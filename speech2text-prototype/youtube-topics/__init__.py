import json
import logging
import string
import os.path
from urllib import request
from urllib.request import urlopen
#import requests
import azure.functions as func
#from youtube_transcript_api import YouTubeTranscriptApi
from simplemma import text_lemmatizer
from collections import Counter
# from urllib.request import urlopen

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    id = req.params.get('id')
    lang = req.params.get('lang')
    if id:
        if not lang:
            lang = 'en'

        #transcript = YouTubeTranscriptApi.get_transcript(id, languages=[lang])
        # data_json = json.loads(transcript, ensure_ascii=False)
        # url = "https://youtubetranscript.azurewebsites.net/api/youtube-transcript?id={0}&lang={1}".format(id, lang)
        # #data_json = request.get(url).json()
        # response = urlopen(url)
        # transcript = str(response.read())
        # logging.info(transcript)
        # data_json = json.loads(transcript)
        # # logging.info(data_json)
        # text = " ".join([item['text'] for item in data_json])

        url = "https://youtubetranscript.azurewebsites.net/api/youtube-transcript?id={0}&lang=uk".format("mlvhJX5V0NM")
        response = urlopen(url)
        transcript = response.read().decode('utf-8')
        data_json = json.loads(transcript)
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
        
        return func.HttpResponse(json.dumps(more_than_once), status_code=200)
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a youtube video id in the query string or in the request body for a transcript.",
             status_code=200
        )
