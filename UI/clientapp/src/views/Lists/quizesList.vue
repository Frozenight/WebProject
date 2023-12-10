<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();
const username = ref(localStorage.getItem('username') || 'Guest');
const quizesCollection = ref([]);
const questionsCollection = ref([]);
const showQuizzes = ref(true);
const localurl = "https://localhost:44340";
const url = "https://quizktuweb.azurewebsites.net";

onMounted(async () => {
    try {
        const response = await axios.get(localurl + "/api/quizes/");
        quizesCollection.value = response.data;
    } catch (error) {
        console.error('Error fetching data:', error);
    }
});

async function fetchQuestions(quizId) {
    try {
        const questionsResponse = await axios.get(localurl + `/api/quizes/${quizId}/questions`);
        questionsCollection.value = await Promise.all(questionsResponse.data.map(async (question) => {
            const answersResponse = await axios.get(localurl + `/api/quizes/${quizId}/questions/${question.id}/answers`);
            return { ...question, answers: answersResponse.data };
        }));
        showQuizzes.value = false;
    } catch (error) {
        console.error('Error fetching questions or answers:', error);
    }
}

function backToQuizzes() {
    showQuizzes.value = true;
}

function logoutAndRedirect() {
    // Clear user-related data
    localStorage.removeItem('username');
    localStorage.removeItem('token');
    username.value = 'Guest';
    // Redirect to login/register page
    router.push({ name: 'login' });
}

function create() {
    router.push({ name: 'admin' });
}

</script>

<template>
    <div class="container">
    <div>
      <p>Logged in as: {{ username }}</p>
      <button @click="logoutAndRedirect">Logout / Back to Login</button>
    </div>
  </div>

    <div class="container">
    <div>
      <p>Admin Menu</p>
      <button @click="create">Go To Admin Menu</button>
    </div>
  </div>
    <!-- Quizzes -->
    <!-- ...existing code... -->
  <div class="container">
    <!-- Quizzes -->
    <div v-if="showQuizzes" class="row row-cols-1 row-cols-md-3 g-4">
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

    <!-- Questions and Answers -->
    <div v-else class="questions">
      <button class="btn btn-secondary" @click="backToQuizzes">Back to Quizzes</button>
      <div v-for="question in questionsCollection" :key="question.id">
        <p>{{ question.text }}</p>
        <div v-for="answer in question.answers" :key="answer.id">
          <button class="btn btn-outline-primary">{{ answer.text }}</button>
        </div>
      </div>  
    </div>
  </div>
</template>
