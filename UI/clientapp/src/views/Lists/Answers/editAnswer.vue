<template>
    <div class="edit-answer-container">
      <h1>Edit Answer</h1>
      <form @submit.prevent="submitAnswer">
        <div>
          <label for="answerText">Answer Text:</label>
          <input type="text" id="answerText" v-model="answer.text" required>
        </div>
        <div>
          <label for="isCorrect">Is Correct:</label>
          <input type="checkbox" id="isCorrect" v-model="answer.isCorrect">
        </div>
        <button type="submit">Update Answer</button>
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
  const answerId = route.params.answerId; // assuming the answer ID is passed as a route parameter
  const answer = ref({
    text: '',
    isCorrect: false,
    QuestiodId: -1
  });
  const localurl = "https://localhost:44340";
  
  onMounted(async () => {
        const token = localStorage.getItem('token');
        if (!token) {
            console.error('No token found');
            return;
        }

        const config = {
            headers: { Authorization: `Bearer ${token}` }
        };

        try {
            const response = await axios.get(`${localurl}/api/quizes/answer/${answerId}`, config);
            answer.value = response.data.resource;
        } catch (error) {
            console.error('Error fetching answer:', error);
        }
    });

  
  async function submitAnswer() {
    try {
      const token = localStorage.getItem('token');
      const config = {
        headers: { Authorization: `Bearer ${token}` }
      };
  
      await axios.put(`${localurl}/api/quizes/answer/${answerId}`, answer.value, config);
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
  