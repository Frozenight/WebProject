<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();
const username = ref(localStorage.getItem('username') || 'Guest');
const userRole = ref(localStorage.getItem('role') || 'Guest'); // Retrieve the role from local storage
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

 function fetchQuestions(quizId) {
  router.push({ name: 'quiz', params: { id: quizId} });
}

function backToQuizzes() {
    showQuizzes.value = true;
}

function logoutAndRedirect() {
    // Clear user-related data
    localStorage.removeItem('username');
    localStorage.removeItem('token');
    localStorage.removeItem('role'); 
    username.value = 'Guest';
    userRole.value = 'Guest';
    router.push({ name: 'login' });
}

function create() {
    router.push({ name: 'admin' });
}

</script>

<template>
  <div class="main-container">
      <!-- Logout Menu -->
      <div class="container logout-menu">
          <p>Logged in as: {{ username }}</p>
          <button class="btn btn-secondary" @click="logoutAndRedirect">Logout / Back to Login</button>
      </div>

      <!-- Admin Menu or Placeholder -->
      <div v-if="userRole === 'Admin'" class="container admin-menu">
          <p>Admin Menu</p>
          <button class="btn btn-primary" @click="create">Go To Admin Menu</button>
      </div>
      <div v-else class="container invisible-placeholder"></div>
  </div>
    <!-- Quizzes -->
    <!-- ...existing code... -->
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
</template>

<style>
.main-container {
    display: flex;
    justify-content: flex-start; /* Aligns children to the start of the container */
    margin: 20px;
}

.container {
    flex-basis: 48%; /* Adjust this value as needed */
    font-family: Arial, sans-serif;
}


.admin-menu {
    margin-left: auto; /* Pushes the admin menu to the right */
}

.invisible-placeholder {
    visibility: hidden;
    width: 500px; /* Keep the same width as the admin-menu for consistency */
}
</style>
