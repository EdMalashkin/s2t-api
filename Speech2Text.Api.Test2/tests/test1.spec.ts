import {test, expect} from '@playwright/test';

// Use test.describe.serial() to group dependent tests to ensure they will always run together and in order. If one of the tests fails, all subsequent tests are skipped. All tests in the group are retried together.
test.describe.configure({ mode: 'serial' });

test.describe('transcript api test', () => {
    test.setTimeout(120000)
    const transcript = {
        "originalURL": "https://www.youtube.com/watch?v=testid",
        "language": "uk"
    }
    const transcriptEdited = transcript;
    transcriptEdited.originalURL += '2';
    transcriptEdited.language = 'en';
    let transcriptid: string;
    const delay = (ms: number) => new Promise(res => setTimeout(res, ms));

    test('should create a transcript', async({ request }) => {   
        const response = await request.post('/transcripts', {data: transcript});
        expect(response.status()).toBe(201);
        const json = await response.json();
        expect(json.id).not.toBeEmpty;
        expect(json.originalURL).toBe(transcript.originalURL);
        expect(json.language).toBe(transcript.language);
        transcriptid = json.id;
        console.log(transcriptid);
    })

    test('should get the transcript', async({ request }) => {
        await delay(20000);
        console.log(`/transcripts/${transcriptid}`);
        const response = await request.get(`/transcripts/${transcriptid}`);
        expect(response.status()).toBe(200);
        const json = await response.json();
        console.log(json);
        expect(json.id).toBe(transcriptid);
        expect(json.originalURL).toBe(transcript.originalURL);
        expect(json.language).toBe(transcript.language);
    });

    test('should update the transcript', async({ request }) => {
        const response = await request.put(`/transcripts/${transcriptid}`, {data: transcriptEdited});
        expect(response.status()).toBe(200);
        const json = await response.json();
        expect(json.id).toBe(transcriptid);
        expect(json.originalURL).toBe(transcriptEdited.originalURL);
        expect(json.language).toBe(transcriptEdited.language);
    })

    test('should get the updated transcript', async({ request }) => {
        await delay(10000);
        console.log(`/transcripts/${transcriptid}`);
        const response = await request.get(`/transcripts/${transcriptid}`);
        expect(response.status()).toBe(200);
        const json = await response.json();
        console.log(json);
        expect(json.id).toBe(transcriptid);
        expect(json.originalURL).toBe(transcriptEdited.originalURL);
        expect(json.language).toBe(transcriptEdited.language);
    })

    test('should delete the transcript', async({ request }) => {
        const response = await request.delete(`/transcripts/${transcriptid}`);
        expect(response.status()).toBe(200);
    })

    test('should not get the deleted transcript', async({ request }) => {
        await delay(10000);
        const response = await request.get(`/transcripts/${transcriptid}`);
        expect(response.status()).toBe(204);
    })

    test.afterAll(async({ request }) => {
        //clean old ever created test transcripts
        const response = await request.delete('/transcripts/', {data: transcript});
        expect(response.ok()).toBeTruthy();
        const response2 = await request.delete('/transcripts/', {data: transcriptEdited});
        expect(response2.ok()).toBeTruthy(); 
    });

});