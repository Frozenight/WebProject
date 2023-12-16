<template>
  <div class="about">
    <h1>About This Project</h1>
    <div class="content">
      <p>This is a front-end quiz with API integration project.</p>
      <p>Developed by <strong>Dainius Juškys</strong>, IFF-0/4 KTU student.</p>
    </div>
    <div class="svg-container">
      <img src="../assets/svg1.svg" alt="Logo" class="svg-icon" />
    </div>
  </div>
</template> 

<script>
export default {
  name: "About",
  // Component-specific JavaScript goes here.
};
</script>

<style scoped>
@import url('https://fonts.google.com/specimen/Rubik+Doodle+Shadow');

.about {
  font-family: 'Rubik Doodle Shadow', cursive;
  margin: 20px;
  padding: 20px;
  text-align: center;
  background-color: #f3f4f6;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.about h1 {
  color: #2c3e50;
  margin-bottom: 20px;
}

.content {
  font-size: 18px;
  line-height: 1.6;
  color: #34495e;
}

.content p {
  margin-bottom: 16px;
}

.content strong {
  font-weight: bold;
  color: #27ae60;
}

.svg-container {
  text-align: center; /* Center the SVG if needed */
}

.svg-icon {
  animation: fadeIn 2s ease-in;
  width: 100px; /* Adjust as needed */
  height: auto;
  /* Additional styling for your SVG */
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

</style>
