<template>
    <div class="add-food mt-4" v-if="foodRecord">
        <h1>Update a Food item</h1>

        <form class="mt-5" @submit.prevent="handleUpdateSubmit">
            <div class="mb-3 mt-3">
                <label class="form-label">Name:</label>
                <input type="text" class="form-control" placeholder="Enter name" v-model="foodRecord.name">
            </div>
            <div class="mb-3">
                <label class="form-label">Value:</label>
                <input type="text" class="form-control" placeholder="Enter value" v-model="foodRecord.value">
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>


            {{ foodRecord.name }}
            {{ foodRecord.value }}
        </form>
    </div>

    <div class="add-food mt-4" v-else>
        <h1>Add a Food item</h1>

        <form class="mt-5" @submit.prevent="handleAddSubmit">
            <div class="mb-3 mt-3">
                <label class="form-label">Name:</label>
                <input type="text" class="form-control" placeholder="Enter name" v-model="name">
            </div>
            <div class="mb-3">
                <label class="form-label">Value:</label>
                <input type="text" class="form-control" placeholder="Enter value" v-model="value">
            </div>
            <button type="submit" class="btn btn-primary">Add</button>
        </form>

        {{name}}
        {{value}}
    </div>
</template>

<script>
import getFoodRecord from '@/composables/getFoodRecord';
import { onMounted, ref } from 'vue';
import ApiService from '@/services/ApiService';


export default {
    props : ['id'],
    setup(props) {
    
        if(props.id) {
            const {foodRecord, error, load} = getFoodRecord(props.id)

            load()


            const handleUpdateSubmit = () => {
                ApiService.update(foodRecord.value, props.id)
                    .then(res => {
                        console.log(res)
                    }).catch(err => {
                        console.log(err.message)
                    })
            }

            return {
                foodRecord, error, handleUpdateSubmit
            }
        }


console.log("adding")
        let name = ref("")
        let value = ref("")

        const handleAddSubmit = () => {

            const foodRecordObj = {
                name: name.value,
                value: value.value,
                dateTime: '2023-06-02T09:19:44.597Z'
            }

            console.log(foodRecordObj)
            ApiService.create(foodRecordObj)
                .then(res => {
                    console.log(res)
                }).catch(err => {
                    console.log(err.response)
                })
        }

        return {
            name, value, handleAddSubmit
        }

        
    }
}
</script>

<style>
.add-food form {
 
    text-align: left;
    width: 550px;
    border-radius: 10px;
    margin: 0 auto;
    padding: 40px;
    background-color: rgb(255, 255, 255);
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.1), 
    0 6px 20px 0 rgba(0, 0, 0, 0.19);}

.add-food form button {
    width: 100%;
}
</style>