const axios = require('axios');

module.exports = async function (context, documents) {
    if (!!documents && documents.length > 0) {
        context.log('Document Id: ', documents[0].id);
    }

    // var promise1 = new Promise(function (resolve, reject) {
    //     setTimeout(function () {
    //         resolve('foo');
    //         context.log('setTimeout: ', documents[0].id);
    //     }, 3000);

    // });
    // context.log(promise1);
    // let resolve = await promise1;
    // context.log(resolve);
    // context.log('resolve: ', documents[0].id);

    documents.forEach(async(doc) => {
        context.log('Document: ', doc);
        const transcript = await GetTranscript2();
        context.log('Trascript', transcript); 
        // try {
        //     const { data } = await axios.get('https://youtubetranscript.azurewebsites.net/api/youtube-transcript', 
        //                         { params: { id: 'CbJjrDjVe2U', lang: 'uk' } });
        //     context.log('data:', data);   
        // } catch (err) {
        //     context.log('err:', err);
        // }
    });
}

async function GetTranscript2(){
    try {
        const { data } = await axios.get('https://youtubetranscript.azurewebsites.net/api/youtube-transcript', 
                            { params: { id: 'CbJjrDjVe2U', lang: 'uk' } })
        return data;
      } catch (err) {
        //console.log(err);
      }
}
//GetTranscript2();

function GetTranscript(){
    axios.get('https://youtubetranscript.azurewebsites.net/api/youtube-transcript', 
                    { params: { id: 'CbJjrDjVe2U', lang: 'uk' } })
            .then(resp => {
                context.log('resp: ', resp);
                //console.log(resp);
            });
}


