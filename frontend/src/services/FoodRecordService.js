import { authHeader } from '../utils/AuthUtils';
import axios from 'axios'

const client = axios.create({
    baseURL: 'https://localhost:7050/api/FoodRecords',
    headers: { 'Content-Type': 'application/json' }
})

const BASE_URL = client.defaults.baseURL;

export default {
    async create(foodRecord) {
        return await axios.post(BASE_URL, foodRecord)
    },
    async getAll(params) {
        console.log("params: ", params)
        return await axios.get(BASE_URL, {params, headers: authHeader()});
    },
    async getById(id) {
        return await axios.get(BASE_URL + `/${id}`, {headers: authHeader()})
    },
    async update(foodRecord, id) {
        return await axios.put(BASE_URL + `/${id}`, foodRecord, {headers: authHeader()})
    },
    async delete(id) {
        return await axios.delete(BASE_URL + `/${id}`, {headers: authHeader()})
    } 
}