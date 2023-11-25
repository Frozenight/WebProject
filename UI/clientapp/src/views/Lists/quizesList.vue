<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

const quizesCollection = ref([]);

onMounted(async () => {
    try {
        const response = await axios.get("https://quizktuweb.azurewebsites.net/api/quizes/");
        quizesCollection.value = response.data;

        // Log the data to the console
        console.log('Quizes Collection:', quizesCollection.value);
    } catch (error) {
        console.error('Error fetching data:', error);
    }
});
</script>

<template>
  <div class="container">
    <div class="row row-cols-1 row-cols-md-3 g-4">
      <div class="col" v-for="item in quizesCollection" :key="item.Id">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">{{item.Name}}</h5>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
