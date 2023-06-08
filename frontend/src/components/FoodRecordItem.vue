<template>
    <div class="food-record-item">
        <div class="content">
            <div class="left">
                <h5>{{foodRecord.name}}</h5>
                <p>{{foodRecord.value}}</p>
                <p>{{foodRecord.foodCategory}}</p>
            </div>
            <div class="right">
     
                <p>{{foodRecord.dateTime}}</p>
                <button class="btn btn-danger" @click="deleteFoodRecord(foodRecord.id)">Delete</button>

                <router-link :to="{name: 'update', params: {id: foodRecord.id}}">
                    <button class="btn btn-success" @click="updateFoodRecord(foodRecord.id)">Update</button>
                </router-link>
                
            </div>
        </div>
        

    </div>
</template>

<script>


export default {
    
    props: ['foodRecord'],
    emits: ['onDeleteClick'],
    setup(props, {emit}) {
        const deleteFoodRecord = (id) => {
            console.log("emitting: " + id)
            emit('onDeleteClick', id);
        }

        const  updateFoodRecord = (id) => {
            console.log("emitting: " + id)
            //emit('onDeleteClick', id);
        }

        return {
            deleteFoodRecord, updateFoodRecord
        }
    }
}
</script>

<style scoped>
    .food-record-item {
        padding: 15px;
        box-shadow: rgba(54, 36, 36, 0.20) 0px 2px 15px;
        border-radius: 10px;
        margin-bottom: 10px;
    }

    .food-record-item .content {
        display: flex;
        justify-content: space-between;
    
    }

    .food-record-item .right {

        margin-top: auto; 
        margin-bottom: 0;

    }

    .food-record-item .left {
  
        flex: 1;
    }

    button {
        font-size: .75rem;
        float: right;
        margin-left: .3rem;
    }


</style>