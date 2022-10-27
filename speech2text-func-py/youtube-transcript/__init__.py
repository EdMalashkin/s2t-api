import json
import logging
import azure.functions as func
from youtube_transcript_api import YouTubeTranscriptApi

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    id = req.params.get('id')
    if not id:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            id = req_body.get('id')

    if id:
        transcript = YouTubeTranscriptApi.get_transcript(id, languages=['uk', 'en'])
        result = json.dumps(transcript, ensure_ascii=False).encode('utf8')
        func.HttpResponse.mimetype = 'application/json'
        func.HttpResponse.charset = 'utf-8'
        return func.HttpResponse(result)
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a youtube video id in the query string or in the request body for a transcript.",
             status_code=200
        )
