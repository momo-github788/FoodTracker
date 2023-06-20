import FoodRecordService from '@/services/FoodRecordService'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'


export default function useFoodRecords() {

    const foodRecord = ref(null)
    const foodRecords = ref([])

    const router = useRouter();

    const errors = ref([]) //array of strings


    const createFoodRecord = async (foodRecord) => {
        errors.value = []
        await FoodRecordService.create(foodRecord)
            .then(res => {
    
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

    
    const searchFoodRecords = async (params) => {
        errors.value = []

        await FoodRecordService.searchFoodRecords(params)
            .then(res => {
                if(!res.status === 200) {
                    throw Error("No data available")
                }
            foodRecords.value = res.data.data;
            console.log("searched: " + foodRecords.value)

            }).catch(err => {
                errors.value.push(err.message)
                console.log("Err: ", err.message)
            })
    }

    const getFoodRecords = (params) => {
        errors.value = []

        FoodRecordService.getAll(params)
            .then(res => {
                if(!res.status === 200) {
                    throw Error("No data available")
                }
            foodRecords.value = res.data.data;
            console.log(res.data)

            }).catch(err => {
                errors.value.push(err.message)
                console.log("Err: ", err.message)
            })
    }
    const getFoodRecord = (id) => {
        errors.value = []

        FoodRecordService.getById(id)
            .then(res => {
                if(!res.status === 200) {
                    throw Error("No data available")
                }
            foodRecord.value = res.data.data;
            console.log(res.data.data)
            }).catch(err => {
                errors.value.push(err.message)
                console.log("Err: ", err.message)
            })
    }

    const updateFoodRecord = (id) => {
        errors.value = []
        FoodRecordService.update(foodRecord.value, id)
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
                console.log(err)
            })
    }

    const deleteFoodRecord = (id) => {
        errors.value = []
        FoodRecordService.delete(id)
            .then(res => {
                console.log(res)
                foodRecords.value = res.data.data
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
        searchFoodRecords,
        updateFoodRecord,
        createFoodRecord,
        deleteFoodRecord
    }

}

