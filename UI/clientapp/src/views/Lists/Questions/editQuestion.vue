<template>
    <div class="edit-question-container">
      <h1>Edit Question</h1>
      <form @submit.prevent="submitQuestion">
        <div>
          <label for="questionText">Question Text:</label>
          <textarea id="questionText" v-model="question.text" required></textarea>
        </div>
        <!-- You might not need to display QuizId in the form, but it should be part of the data sent to the server -->
        <button type="submit">Update Question</button>
        <button type="button" @click="goBack">Back</button>
      </form>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import { useRouter, useRoute } from 'vue-router';
  
  const router = useRouter();
  const route = useRoute();
  const localurl = "https://localhost:44340";
  const questionId = route.params.id;
  const question = ref({
    text: '',
    QuizId: -1
  });

  onMounted(async () => {
    const token = localStorage.getItem('token');
    const config = {
      headers: { Authorization: `Bearer ${token}` }
    };
  
    try {
      const response = await axios.get(`${localurl}/api/quizes/question/${questionId}`, config);
      question.value = response.data.resource;
    } catch (error) {
      console.error('Error fetching question:', error);
    }
  });
  
  async function submitQuestion() {
    try {
      const token = localStorage.getItem('token');
      const config = {
        headers: { Authorization: `Bearer ${token}` }
      };
  
      await axios.put(`${localurl}/api/quizes/question/${questionId}`, question.value, config);
      router.back();
    } catch (error) {
      console.error('Error updating question:', error);
    }
  }
  
  function goBack() {
    router.back();
  }
  </script>
  
  <style scoped>
  /* Add your CSS here */
  </style>
  