<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();
const username = ref(localStorage.getItem('username') || 'Guest');
const showModal = ref(username.value === 'Guest');
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

function redirectToLogin() {
  router.push({ name: 'login' });
  showModal.value = false;
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

    <div v-if="showModal" class="modal-overlay" @click="showModal = false">
    <div class="modal-content" @click.stop>
      <span class="close" @click="showModal = false">&times;</span>
      <p>Please log in.</p>
      <button class="btn btn-primary" @click="redirectToLogin">Login</button>
    </div>
  </div>
</template>

<style>
.main-container {
    display: flex;
    justify-content: flex-start;
    margin: 20px;
}

.container {
    flex-basis: 48%;
    font-family: Arial, sans-serif;
}

.admin-menu {
    margin-left: auto;
}

.invisible-placeholder {
    visibility: hidden;
    width: 500px;
}

@media (max-width: 1024px) {
    .row-cols-md-3 {
        -ms-flex: 0 0 100%;
        flex: 0 0 100%;
        max-width: 100%;
    }

    .col {
        flex-basis: 100%;
        max-width: 100%;
    }
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.6);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  background-color: white;
  padding: 20px;
  width: 50%;
  border-radius: 5px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  position: relative;
}

.close {
  position: absolute;
  top: 10px;
  right: 10px;
  font-size: 24px;
  cursor: pointer;
}
</style>