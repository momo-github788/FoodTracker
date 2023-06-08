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
    async getAll() {
        return await axios.get(BASE_URL);
    },
    async searchFoodRecords(params) {
        console.log("params: ", params)
        return await axios.get(BASE_URL + `/searchQuery`, {params});
    },
    async getById(id) {
        return await axios.get(BASE_URL + `/${id}`)
    },
    async update(foodRecord, id) {
        return await axios.put(BASE_URL + `/${id}`, foodRecord)
    },
    async delete(id) {
        return await axios.delete(BASE_URL + `/${id}`)
    } 
}