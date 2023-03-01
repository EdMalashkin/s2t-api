import json
import logging
import string
import os.path
import azure.functions as func
from simplemma import text_lemmatizer
from collections import Counter
from urllib.request import urlopen

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    id = req.params.get('id')
    lang = req.params.get('lang')
    if id:
        if not lang:
            lang = 'en'

        url = "https://youtubetranscript.azurewebsites.net/api/youtube-transcript?id={0}&lang={1}".format(id, lang)
        response = urlopen(url)
        transcript = str(response.read())
        data_json = json.loads(transcript)
        logging.info(data_json)
        return data_json
        text = " ".join([item['text'] for item in data_json])

        stopfilelink = os.path.join(os.path.dirname(__file__), 'stopwords_ua_list.txt') #https://github.com/skupriienko/Ukrainian-Stopwords/blob/master/stopwords_ua_list.txt
        doc = open(stopfilelink, encoding ='utf-8') 
        stopwords = doc.read()
        doc.close()

        lemmatized = text_lemmatizer(text, lang='uk')
        lowcased = list(map(lambda x: x.lower(), lemmatized))
        cleaned = [word for word in lowcased if not word in stopwords]

        c = Counter(cleaned)
        words_frequency = c.most_common()
        more_than_once = [f for f in words_frequency if f[1] > 1]
        return more_than_once
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a youtube video id in the query string or in the request body for a transcript.",
             status_code=200
        )
