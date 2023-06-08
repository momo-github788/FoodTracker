<template>
  <div class="sort-container">
    <v-select :options="options" label="title"  >
    </v-select>  
  </div>
</template>

<script>


export default {

    setup() {
       const options = [
      {
          title: 'Read the Docs',
          icon: 'fa-book',
          url: 'https://codeclimate.com/github/sagalbot/vue-select'
        },
        {
          title: 'View on GitHub',
          icon: 'fa-github',
          url: 'https://codeclimate.com/github/sagalbot/vue-select'
        },
        {
          title: 'View on NPM',
          icon: 'fa-database',
          url: 'https://codeclimate.com/github/sagalbot/vue-select'
        },
        {
          title: 'View Codepen Examples',
          icon: 'fa-pencil',
          url: 'https://codeclimate.com/github/sagalbot/vue-select'
        }
    ]
        
  
        return {
             options
        }
    }
}

</script>

<style>

</style>