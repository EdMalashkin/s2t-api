const request = require('supertest');
const expect = require('chai').expect;
const transcript = require('./testdata/newtranscript.json');
const transcriptEdited = transcript;
transcriptEdited.originalURL += '2';
transcriptEdited.language = 'en';

describe('Restful Speech2Test API Tests', async () => {
    const baseurl = 'https://speech2text-web.azurewebsites.net';
    let transcriptid;
    // 1
    it('should successfully create a transcript', async () => {
        const res = await request(baseurl)
        .post('/transcripts')
        .send(transcript)        
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        expect(res.statusCode).to.be.equal(201);
        expect(res.body.originalURL).to.be.equal(transcript.originalURL);
        expect(res.body.language).to.be.equal(transcript.language);
        expect(res.body.id).not.to.be.null;
        transcriptid = res.body.id; 
    });
    // 2
    it('should fetch the transcript of the provided transcript id', async () => {
        await sleep(10000); //to avoid 204 status code
        //const transcriptid = "d91fd098-6184-42fc-81d1-931b4dc4094a"
        console.log("transcriptid", transcriptid)
        const res = await request(baseurl)
        .get('/transcripts/' + transcriptid)        
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')               
        expect(res.statusCode).to.be.equal(200);
        expect(res.body.originalURL).to.be.equal(transcript.originalURL);
        expect(res.body.language).to.be.equal(transcript.language);
        expect(res.body.id).to.be.equal(transcriptid);
    });  
    // 3
    it('should update the transcript of the provided transcript id', async () => {
        const res = await request(baseurl)
        .put('/transcripts/' + transcriptid)
        .send(transcriptEdited)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        expect(res.statusCode).to.be.equal(200);
        expect(res.body.originalURL).to.be.equal(transcriptEdited.originalURL);
        expect(res.body.language).to.be.equal(transcriptEdited.language);
        expect(res.body.id).to.be.equal(transcriptid);
    });
    // 4
    it('should fetch the transcript of the updated transcript id', async() => {
        const res = await request(baseurl)
        .get('/transcripts/' + transcriptid)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        expect(res.statusCode).to.be.equal(200);
        expect(res.body.originalURL).to.be.equal(transcriptEdited.originalURL);
        expect(res.body.language).to.be.equal(transcriptEdited.language);
        expect(res.body.id).to.be.equal(transcriptid);
    });
    // 5
    it('should delete the transcript of the updated transcript id', async () => {
        const res = await request(baseurl)
        .delete('/transcripts/' + transcriptid)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        expect(res.statusCode).to.be.equal(200);
    });
    // 6
    it('should fetch the transcript of the deleted transcript id', async () => {
        const res = await request(baseurl)
        .get('/transcripts/' + transcriptid)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        expect(res.statusCode).to.be.equal(204);
    });
    // 7
    it('should clean old test original transcripts', async () => {
        const res = await request(baseurl)
        .delete('/transcripts/')
        .send(transcript)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        expect(res.statusCode).to.be.equal(200);
    });
    // 8
    it('should clean old test edited transcripts', async () => {
        const res = await request(baseurl)
        .delete('/transcripts/')
        .send(transcriptEdited)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        expect(res.statusCode).to.be.equal(200);
    });       
});

function sleep(ms) {
    return new Promise((resolve) => {
      setTimeout(resolve, ms);
    });
}