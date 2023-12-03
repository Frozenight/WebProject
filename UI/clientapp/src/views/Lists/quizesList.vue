<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

const quizesCollection = ref([]);
const questionsCollection = ref([]);

onMounted(async () => {
    try {
        const response = await axios.get("https://quizktuweb.azurewebsites.net/api/quizes/");
        quizesCollection.value = response.data;
        console.log('Quizes Collection:', quizesCollection.value);
    } catch (error) {
        console.error('Error fetching data:', error);
    }
});

async function fetchQuestions(quizId) {
    try {
        const response = await axios.get(`https://quizktuweb.azurewebsites.net/api/quizes/${quizId}/questions`);
        questionsCollection.value = response.data;
        console.log('Questions for quiz', quizId, ':', questionsCollection.value);
        // Here you can handle the display or usage of the questions
    } catch (error) {
        console.error('Error fetching questions:', error);
    }
}
</script>

<template>
  <div class="container">
    <div class="row row-cols-1 row-cols-md-3 g-4">
      <div class="col" v-for="quiz in quizesCollection" :key="quiz.id">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">{{ quiz.name }}</h5>
            <p class="card-text"><strong>Description:</strong> {{ quiz.description }}</p>
            <p class="card-text"><strong>Category:</strong> {{ quiz.category }}</p>
            <button class="btn btn-primary" @click="fetchQuestions(quiz.id)">Start Quiz</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>