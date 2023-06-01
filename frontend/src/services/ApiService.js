import axios from 'axios'
import vue from 'vue'
i

const client = axios.create({
    baseURL: 'http://localhost:5000/api/FoodRecords',
    json: true
})

export default {
    getAll() {
        console.log(client.baseURL)
    },
      create(data) {
        return this.execute('post', '/', data)
    },
    update(id, data) {
        return this.execute('put', `/${id}`, data)
    },
    delete(id) {
        return this.execute('delete', `/${id}`)
    } 
}