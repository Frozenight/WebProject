<template>
  <div class="questions">
    <button class="btn btn-secondary" @click="$router.go(-1)">Back to Quizzes</button>

    <div v-if="questionsCollection.length > 0 && !quizCompleted">
      <div :key="questionsCollection[currentQuestionIndex].id">
        <p>{{ questionsCollection[currentQuestionIndex].text }}</p>
        <div v-for="answer in questionsCollection[currentQuestionIndex].answers" :key="answer.id">
          <button class="btn btn-outline-primary" @click="handleAnswerClick(answer)">{{ answer.text }}</button>
        </div>
      </div>
    </div>

    <div v-if="quizCompleted">
      <h3>Quiz Results</h3>
      <p>Correct Answers: {{ calculateResults().correctAnswers }}</p>
      <p>Incorrect Answers: {{ calculateResults().incorrectAnswers }}</p>
      <!-- Add any other results or navigation buttons here -->
    </div>
  </div>
  <div class="svg-container">
      <img src="../../assets/svg2.svg" alt="Logo" class="svg-icon" />
    </div>
    <div class="svg-container1">
      <img src="../../assets/svg3.svg" alt="Logo" class="svg-icon" />
    </div>
    <div class="svg-container2">
      <img src="../../assets/svg4.svg" alt="Logo" class="svg-icon" />
    </div>
</template>


<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';
import { useRouter, useRoute } from 'vue-router';

const router = useRouter();
const route = useRoute();
const questionsCollection = ref([]);
const currentQuestionIndex = ref(0);
const userAnswers = ref([]); // Array to keep track of user's answers
const quizCompleted = ref(false); // Indicates if the quiz is completed
const localurl = "https://localhost:44340";

const quizId = route.params.id;

onMounted(async () => {
  try {
    const questionsResponse = await axios.get(`${localurl}/api/quizes/${quizId}/questions`);
    questionsCollection.value = await Promise.all(questionsResponse.data.map(async (question) => {
      const answersResponse = await axios.get(`${localurl}/api/quizes/${quizId}/questions/${question.id}/answers`);
      return { ...question, answers: answersResponse.data };
    }));
  } catch (error) {
    console.error('Error fetching questions or answers:', error);
  }
});

const handleAnswerClick = (answer) => {
  userAnswers.value.push(answer.isCorrect);

  if (currentQuestionIndex.value < questionsCollection.value.length - 1) {
    currentQuestionIndex.value++;
  } else {
    quizCompleted.value = true;
    // Here you might want to navigate to a results page or trigger a modal, etc.
  }
};

// Function to calculate results
const calculateResults = () => {
  const correctAnswers = userAnswers.value.filter(answer => answer).length;
  const incorrectAnswers = userAnswers.value.length - correctAnswers;
  return { correctAnswers, incorrectAnswers };
};
</script>

<style scoped>
.questions {
  max-width: 600px;
  margin: 2rem auto;
  padding: 1rem;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
  background-color: #f9f9f9;
}

.questions button {
  display: block;
  width: 100%;
  padding: 0.5rem 1rem;
  margin: 0.5rem 0;
  border: none;
  border-radius: 5px;
  background-color: #007bff;
  color: white;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.questions button:hover {
  background-color: #0056b3;
}

.questions p {
  font-size: 1.2rem;
  color: #333;
}

.results {
  text-align: center;
}

.results h3 {
  color: #007bff;
  margin-bottom: 1rem;
}

.results p {
  font-size: 1rem;
  color: #333;
}

.svg-container {
  text-align: right; /* Center the SVG if needed */
}

.svg-container1 {
  text-align: left; /* Center the SVG if needed */
}

.svg-container2 {
  text-align: center; /* Center the SVG if needed */
}

.svg-icon {
  width: 100px; /* Adjust as needed */
  height: auto;
}
</style>
