import axios from 'axios'

const client = axios.create({
    baseURL: 'https://localhost:7050/api/auth',
    headers: { 'Content-Type': 'application/json' }
})

const BASE_URL = client.defaults.baseURL;

export default {
    async register(request) {
        return await axios.post(BASE_URL + "/register", request)
    },
    async login(request) {
        return await axios.post(BASE_URL + "/login", request);
    }
}