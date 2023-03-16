import json
import logging
import os.path
import azure.functions as func
import pymorphy2
from urllib import request
from urllib.request import urlopen
from collections import Counter

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    id = req.params.get('id')
    lang = req.params.get('lang')
    if id:
        if not lang:
            lang = 'en'
        
        # 1. Get a transcript
        url = "https://youtubetranscript.azurewebsites.net/api/youtube-transcript?id={0}&lang={1}".format(id, lang)
        response = urlopen(url)
        transcript = response.read().decode('utf-8')
        data_json = json.loads(transcript)
        text = " ".join([item['text'] for item in data_json])

        # 2. Lemmatize the transcript
        #lemmatized = text_lemmatizer(text, lang=lang)
        morph = pymorphy2.MorphAnalyzer(lang=lang)
        words = text.split()
        lemmatized = []
        for word in words:
            lemma = morph.parse(word)[0].normal_form
            lemmatized.append(lemma)
       
        # 3. Clean the transcript
        stopfilelink = os.path.join(os.path.dirname(__file__), 'stopwords_ua_list.txt') #https://github.com/skupriienko/Ukrainian-Stopwords/blob/master/stopwords_ua_list.txt
        doc = open(stopfilelink, encoding ='utf-8')
        stopwords = doc.read()
        doc.close()

        lowcased = list(map(lambda x: x.lower(), lemmatized))
        cleaned = [word for word in lowcased if not word in stopwords]

        # 4. Build word usage statistics
        c = Counter(cleaned)
        words_frequency = c.most_common()
        more_than_once = [f for f in words_frequency if f[1] > 1]
        
        func.HttpResponse.mimetype = 'application/json'
        func.HttpResponse.charset = 'utf-8'
        return func.HttpResponse(json.dumps(more_than_once), status_code=200, )
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a youtube video id in the query string or in the request body for a transcript.",
             status_code=200
        )
