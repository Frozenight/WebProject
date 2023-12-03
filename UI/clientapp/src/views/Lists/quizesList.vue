<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

const quizesCollection = ref([]);
const questionsCollection = ref([]);
const showModal = ref(false);

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
        showModal.value = true;
    } catch (error) {
        console.error('Error fetching questions:', error);
    }
}

function closeQuiz() {
    showModal.value = false;
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

  <!-- Quiz Modal -->
  <div v-if="showModal" class="quiz-modal">
    <h2>Quiz Questions</h2>
    <div v-for="question in questionsCollection" :key="question.id">
      <p>{{ question.text }}</p>
    </div>
    <button class="btn btn-secondary" @click="closeQuiz">Close</button>
  </div>
</template>

<style>
.quiz-modal {
  /* Add your modal styling here */
}
</style>
