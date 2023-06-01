<template>
    <div class="add-food mt-4" v-if="foodRecord">
        <h1>Update a Food item</h1>

        <form class="mt-5" @submit.prevent="handleUpdate">
            <div class="mb-3 mt-3">
                <label class="form-label">Name:</label>
                <input type="text" class="form-control" placeholder="Enter name" :v-model="foodRecord.name" :value="foodRecord.name">
            </div>
            <div class="mb-3">
                <label class="form-label">Value:</label>
                <input type="text" class="form-control" placeholder="Enter bvalue" :v-model="foodRecord.value" :value="foodRecord.value">
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>

            {{ foodRecord.name }}
            {{ foodRecord.value }}
        </form>
    </div>

    <div class="add-food mt-4" v-else>
        <h1>Add a Food item</h1>

        <form class="mt-5">
            <div class="mb-3 mt-3">
                <label for="email" class="form-label">Email:</label>
                <input type="email" class="form-control" id="email" placeholder="Enter email">
            </div>
            <div class="mb-3">
                <label for="pwd" class="form-label">Password:</label>
                <input type="text" class="form-control" id="pwd" placeholder="Enter password">
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
        </form>
    </div>
</template>

<script>
import getFoodRecord from '@/composables/getFoodRecord';


export default {
    props : ['id'],
    setup(props) {
    
        const {foodRecord, error, load} = getFoodRecord(props.id)

        load()


        const handleUpdate = () => {
            console.log("updated: ", foodRecord.value)
        }

       

        return {
            foodRecord, error, handleUpdate
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