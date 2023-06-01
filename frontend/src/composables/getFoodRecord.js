import ApiService from '../services/ApiService';
import { onMounted, ref } from 'vue'

const getFoodRecord = (id) => {

    let foodRecord = ref(null)
    const error = ref(null)


    const load = () => {
      ApiService.getById(id)
        .then(res => {
            if(!res.status === 200) {
                throw Error("No data available")
            }
          foodRecord.value = res.data;
        }).catch(err => {
            error.value = err.message
            console.log("Err: ", err.message)
        })
    }

    return {
        foodRecord, error, load
    }

}

export default getFoodRecord