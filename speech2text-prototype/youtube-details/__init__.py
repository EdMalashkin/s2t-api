import json
import logging
import os.path
import azure.functions as func
import pymorphy2
from urllib import request
from urllib.request import urlopen
from simplemma import text_lemmatizer
from collections import Counter

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    id = req.params.get('id')
    lang = req.params.get('lang')
    if id:
        if not lang:
            lang = 'en'
        
        url = "https://youtubetranscript.azurewebsites.net/api/youtube-transcript?id={0}&lang={1}".format(id, lang)
        response = urlopen(url)
        transcript = response.read().decode('utf-8')
        data_json = json.loads(transcript)

        stopfilelink = os.path.join(os.path.dirname(__file__), 'stopwords_ua_list.txt') #https://github.com/skupriienko/Ukrainian-Stopwords/blob/master/stopwords_ua_list.txt
        doc = open(stopfilelink, encoding ='utf-8')
        stopwords = doc.read()
        doc.close()

        morph = pymorphy2.MorphAnalyzer(lang=lang)

        for item in data_json:
            #text = item['text'].replace("'","")
            #lemmatized = text_lemmatizer(text, lang=lang, greedy=False)
            words = item['text'].split()
            lowered = [word.lower() for word in words]
            cleaned = [word for word in lowered if not word in stopwords]
            item['cleaned'] = " ".join(cleaned)
            lemmatized = []
            for word in cleaned:
                lemma = morph.parse(word)[0].normal_form
                lemmatized.append(lemma)
            item['lemmatized'] = " ".join([word for word in lemmatized])
            #item['clean'] = " ".join([word for word in lemmatized if not word in stopwords])

        func.HttpResponse.mimetype = 'application/json'
        func.HttpResponse.charset = 'utf-8'
        return func.HttpResponse(json.dumps(data_json), status_code=200, )
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a youtube video id in the query string or in the request body for a transcript.",
             status_code=200
        )
