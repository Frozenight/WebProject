<template>
    <div class="edit-question-container">
      <h1>Edit Question</h1>
      <form @submit.prevent="submitQuestion">
        <div>
          <label for="questionText">Question Text:</label>
          <input type="text" id="questionText" v-model="question.text" required>
        </div>
        <!-- You might not need to display QuizId in the form, but it should be part of the data sent to the server -->
        <button  class="btn btn-primary"  type="submit">Update Question</button>
        <button  class="btn btn-secondary"   type="button" @click="goBack">Back</button>
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
       .container {
            font-family: Arial, sans-serif;
            margin: 20px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }

        input[type="text"] {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        input[type="checkbox"] {
            margin-right: 10px;
        }
  </style>
  