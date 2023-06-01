<template>
    <div class="add-food mt-4">

        <h1>Add a Food item</h1>

        <form class="mt-5">
            <div class="mb-3 mt-3">
                <label for="email" class="form-label">Email:</label>
                <input type="email" class="form-control" id="email" placeholder="Enter email" name="email">
            </div>
            <div class="mb-3">
                <label for="pwd" class="form-label">Password:</label>
                <input type="password" class="form-control" id="pwd" placeholder="Enter password" name="pswd">
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