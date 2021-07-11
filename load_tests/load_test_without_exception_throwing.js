import http from 'k6/http';

let endpoint = "without";

export let options = {
    insecureSkipTLSVerify: true,
    vus: 100,
    duration: '1m'
};

export default function () {
    http.get(`http://localhost:5000/test/${endpoint}`, { responseCallback: null});
}
