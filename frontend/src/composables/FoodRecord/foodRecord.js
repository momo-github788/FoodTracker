import ApiService from '@/services/ApiService'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'


export default function useFoodRecords() {

    const foodRecord = ref(null)
    const foodRecords = ref([])

    const router = useRouter();

    const errors = ref([]) //array of strings


    const createFoodRecord = (foodRecord) => {
        errors.value = []
        ApiService.create(foodRecord)
            .then(res => {
                console.log("RES: ", res)
                if(res.status !== 200) {
                    throw Error("Error adding")
                }
                router.push({
                    name: 'home'
                })
            }).catch(err => {
                console.log(err.response.data.errors.request)
                errors.value.push(err.response.data.errors.request)
            })
    }

    const getFoodRecords = () => {
        errors.value = []

        ApiService.getAll()
            .then(res => {
                if(!res.status === 200) {
                    throw Error("No data available")
                }
            foodRecords.value = res.data;
            }).catch(err => {
                errors.value.push(err.message)
                console.log("Err: ", err.message)
            })
    }
    const getFoodRecord = (id) => {
        errors.value = []

        ApiService.getById(id)
            .then(res => {
                if(!res.status === 200) {
                    throw Error("No data available")
                }
            foodRecord.value = res.data;
            }).catch(err => {
                errors.value.push(err.message)
                console.log("Err: ", err.message)
            })
    }

    const updateFoodRecord = (id) => {
        errors.value = []
        ApiService.update(foodRecord.value, id)
            .then(res => {
                console.log(res)
                if(!res.status === 200) {
                    throw Error("Error updating")
                }
                router.push({
                    name: 'home'
                })
            }).catch(err => {
                errors.value.push(err.message)
                console.log(err.message)
            })
    }

    const deleteFoodRecord = (id) => {
        errors.value = []
        ApiService.delete(id)
            .then(res => {
                console.log(res)

            }).catch(err => {
                console.log("err: ", err.message)
                errors.value.push(err.message)
            })
    }


    
    return {
        errors,
        foodRecord,
        foodRecords, 
        getFoodRecords,
        getFoodRecord,
        updateFoodRecord,
        createFoodRecord,
        deleteFoodRecord
    }

}

