const request = require('supertest');
const expect = require('chai').expect;
const transcript = require('./testdata/newtranscript.json');

describe('Restful Speech2Test API Tests', () => {
    const baseurl = 'https://speech2text-web.azurewebsites.net';
    var transcriptid;
    it('should successfully create a transcript', (done) => {
        request(baseurl)
        .post('/transcripts')
        .send(transcript)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        .end(function(err, res){
            expect(res.statusCode).to.be.equal(201);
            expect(res.body.originalURL).to.be.equal(transcript.originalURL);
            expect(res.body.language).to.be.equal(transcript.language);
            expect(res.body.id).not.to.be.null;
            transcriptid = res.body.id;
            if(err){
                throw err;
            }
            done();
        })
    });
    it('should fetch the transcript of the provided transcript id', (done) => {
        request(baseurl)
        .get('/transcripts/' + transcriptid)
        .send(transcript)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        .end(function(err, res){
            expect(res.statusCode).to.be.equal(200);
            expect(res.body.originalURL).to.be.equal(transcript.originalURL);
            expect(res.body.language).to.be.equal(transcript.language);
            expect(res.body.id).to.be.equal(transcriptid);
            if(err){
                throw err;
            }
            done();
        })
    });
    it('should update the transcript of the provided transcript id', (done) => {
        transcript.originalURL += '2';
        transcript.language = 'en';
        request(baseurl)
        .put('/transcripts/' + transcriptid)
        .send(transcript)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        .end(function(err, res){
            expect(res.statusCode).to.be.equal(200);
            expect(res.body.originalURL).to.be.equal(transcript.originalURL);
            expect(res.body.language).to.be.equal(transcript.language);
            expect(res.body.id).to.be.equal(transcriptid);
            if(err){
                throw err;
            }
            done();
        })
    });
    it('should fetch the transcript of the updated transcript id', (done) => {
        request(baseurl)
        .get('/transcripts/' + transcriptid)
        .send(transcript)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        .end(function(err, res){
            expect(res.statusCode).to.be.equal(200);
            expect(res.body.originalURL).to.be.equal(transcript.originalURL);
            expect(res.body.language).to.be.equal(transcript.language);
            expect(res.body.id).to.be.equal(transcriptid);
            if(err){
                throw err;
            }
            done();
        })
    });  
    it('should delete the transcript of the updated transcript id', (done) => {
        request(baseurl)
        .delete('/transcripts/' + transcriptid)
        .send(transcript)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        .end(function(err, res){
            expect(res.statusCode).to.be.equal(200);
            if(err){
                throw err;
            }
            done();
        })
    });
    it('should fetch the transcript of the deleted transcript id', (done) => {
        request(baseurl)
        .get('/transcripts/' + transcriptid)
        .send(transcript)
        .set('Accept', 'application/json')
        .set('Content-Type', 'application/json')
        .end(function(err, res){
            expect(res.statusCode).to.be.equal(204);
            if(err){
                throw err;
            }
            done();
        })
    });            
});
