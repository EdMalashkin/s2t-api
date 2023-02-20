const axios = require('axios');

module.exports = async function (context, documents) {
    documents.forEach(async(doc) => {
        console.log('Document: ', doc);
        const transcript = await GetTranscript();
        doc.data = transcript;
        console.log('Document with transcript', doc);
        await SaveTranscript(doc);
    });
}

async function GetTranscript(){
    try {
        const { data } = await axios.get('https://youtubetranscript.azurewebsites.net/api/youtube-transcript', 
                            { params: { id: 'CbJjrDjVe2U', lang: 'uk' } })
        console.log('GetTranscript', data);                            
        return data;
      } catch (err) {
        console.log(err);
      }
}


async function SaveTranscript(doc){
    try {
        const url = 'https://speech2text-web.azurewebsites.net/youtubetranscripts/' + doc.id;
        console.log(url);
        const config = { 'content-type': 'application/json' };
        const response = await axios.post(url, doc, config);
        console.log('SaveTranscript', response.data);
    } catch (err) {
        console.error(err);
    }
}

async function Test() {
    const doc = {
        "id": "c713a139-a6a3-4596-a9c2-c16de48c04a5",
        "OriginalURL": "https://www.youtube.com/watch?v=CbJjrDjVe3U",
        "Language": "uk"
    };
    await SaveTranscript(doc);
}
//Test();