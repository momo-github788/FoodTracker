<template>
    <div class="container-fluid mt-4">
        <h1 class="h1">Food Records</h1>
        <table class="table">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Value</th>
                    <th>Date Time</th>
                    <th>&nbsp;</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <!-- <td>{{ record.id }}</td>
                    <td>{{ record.name }}</td>
                    <td>{{ record.value }}</td>
                    <td>{{ record.dateTime }}</td> -->
                    <!-- <td class="text-right">
                        <a href="#" @click.prevent="updateFoodRecord(record)">Edit</a> -
                        <a href="#" @click.prevent="deleteFoodRecord(record.id)">Delete</a>
                    </td> -->
                </tr>
            </tbody>
        </table>

        <form>
            <div class="mb-3">
                <label for="exampleInputEmail1" class="form-label">Email address</label>
                <input type="email" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp">
                <div id="emailHelp" class="form-text">We'll never share your email with anyone else.</div>
            </div>
            <div class="mb-3">
                <label for="exampleInputPassword1" class="form-label">Name</label>
                <input type="password" class="form-control" id="exampleInputPassword1">
            </div>
            <div class="mb-3 form-check">
                <input type="checkbox" class="form-check-input" id="exampleCheck1">
                <label class="form-check-label" for="exampleCheck1">Value</label>
            </div>
            <div class="mb-3">
                <label for="exampleInputPassword1" class="form-label">Date Time</label>
                <input type="datetime-local" class="form-control" id="exampleInputPassword1">
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
        </form>

    </div>
    
</template>

<script>

</script>

<style scoped>
    form {
        width: 550px;
        margin: 0 auto;
        background-color: aliceblue;
    }
</style>