import ApiService from '../services/ApiService';
import { onMounted, ref } from 'vue'

const getFoodRecords = () => {

    let foodRecords = ref([])
    const error = ref(null)


    const load = () => {
      ApiService.getAll()
        .then(res => {
            if(!res.status === 200) {
                throw Error("No data available")
            }
          foodRecords.value = res.data;
        }).catch(err => {
            error.value = err.message
            console.log("Err: ", err.message)
        })
    }

    return {
        foodRecords, error, load
    }

}

export default getFoodRecords