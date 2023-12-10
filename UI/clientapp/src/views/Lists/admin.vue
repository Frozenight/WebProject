<template>
  <div class="admin-container">
    <h1>Admin Dashboard</h1>

    <button @click="createQuiz">Create New Quiz</button>

    <div class="quizzes-list">
      <div class="col" v-for="quiz in quizesCollection" :key="quiz.id">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">{{ quiz.name }}</h5>
            <p class="card-text"><strong>Description:</strong> {{ quiz.description }}</p>
            <p class="card-text"><strong>Category:</strong> {{ quiz.category }}</p>
            <button class="btn btn-primary" @click="editQuiz(quiz.id)">Edit</button>
            <button class="btn btn-primary" @click="deleteQuiz(quiz.id)">Delete</button>
            <button class="btn btn-primary" @click="editQuestions(quiz.id)">Edit Questions</button>
          </div>
      </div>
    </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();
const quizesCollection = ref([]);
const localurl = "https://localhost:44340";

// Fetch quizzes
onMounted(async () => {
  try {
    const response = await axios.get(localurl + "/api/quizes/");
    quizesCollection.value = response.data;
  } catch (error) {
    console.error('Error fetching quizzes:', error);
  }
});

// Create a new quiz
function createQuiz() {
  router.push({ name: 'createQuiz' });
}

// Edit a quiz
function editQuiz(quizId) {
  router.push({ name: 'editQuiz', params: { id: quizId } });
}

// Delete a quiz
async function deleteQuiz(quizId) {
  try {
    const token = localStorage.getItem('token');
    await axios.delete(localurl + `/api/quizes/${quizId}`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    window.location.reload()
    // Remove the quiz from quizesCollection or refetch the list
  } catch (error) {
    console.error('Error deleting quiz:', error);
  }
}

function editQuestions(quizId) {
    router.push({ name: 'editQuestions', params: { id: quizId } });
  }

</script>

<style scoped>
/* Add your CSS here */
</style>
