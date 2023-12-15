<template>
    <div class="create-answer-container">
      <h1>Create New Answer for Question ID: {{ questionId }}</h1>
      <form @submit.prevent="submitAnswer">
        <div>
          <label for="answerText">Answer Text:</label>
          <input type="text" id="answerText" v-model="answer.text" required>
        </div>
        <div>
          <label for="isCorrect">Is Correct:</label>
          <input type="checkbox" id="isCorrect" v-model="answer.isCorrect">
        </div>
        <button class="btn btn-primary" type="submit">Create Answer</button>
        <button class="btn btn-secondary" type="button" @click="goBack">Back</button>
      </form>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import axios from 'axios';
  import { useRouter, useRoute } from 'vue-router';
  
  const router = useRouter();
  const route = useRoute();
  const questionId = route.params.id; // Make sure this is passed from the parent component
  const quizId = route.params.quizId;
  const answer = ref({
    text: '',
    isCorrect: false
  });
  const localurl = "https://localhost:44340";
  
  async function submitAnswer() {
    try {
      const token = localStorage.getItem('token');
      const config = {
        headers: { Authorization: `Bearer ${token}` }
      };
  
      const answerData = {
        Text: answer.value.text,
        IsCorrect: answer.value.isCorrect,
        QuestionId: questionId
      };
  
      await axios.post(`${localurl}/api/quizes/answer`, answerData, config);
  
      // Handle successful creation
      router.push({ name: 'editAnswers', params: { quizId: quizId, questionId: questionId } });
    } catch (error) {
      console.error('Error creating answer:', error);
      // Handle errors
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
  