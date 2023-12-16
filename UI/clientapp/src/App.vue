  <script setup>
  import { RouterLink, RouterView } from 'vue-router'
  import { ref } from 'vue';

  const isMenuVisible = ref(false);

  function toggleMenu() {
    isMenuVisible.value = !isMenuVisible.value;
  }
  </script>

<template>
  <div>
    <nav class="navbar bg-primary" data-bs-theme="dark">
      <div class="container-fluid">
        <!-- Hamburger Menu Button -->
        <button class="navbar-toggler" type="button" @click="toggleMenu">
          <span class="navbar-toggler-icon"></span>
        </button>

        <!-- Links for Larger Screens -->
        <div class="navbar-links">
          <RouterLink to="/" class="nav-link">Quizes</RouterLink>
          <RouterLink to="/about" class="nav-link">About</RouterLink>
        </div>

        <!-- Collapsible Menu Items for Smaller Screens -->
        <div v-if="isMenuVisible" class="navbar-collapse">
          <ul class="navbar-nav">
            <li class="nav-item">
              <RouterLink to="/" class="nav-link">Quizes</RouterLink>
            </li>
            <li class="nav-item">
              <RouterLink to="/about" class="nav-link">About</RouterLink>
            </li>
          </ul>
        </div>
      </div>
    </nav>
    <div class="container">
      <RouterView />
    </div>
  </div>
</template>
<style scoped>
.navbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.navbar-toggler {
  border: none;
  background-color: transparent;
}

.navbar-toggler-icon {
  background-image: url('data:image/svg+xml;charset=utf8,%3Csvg viewBox="0 0 30 30" xmlns="http://www.w3.org/2000/svg"%3E%3Cpath stroke="rgba(255, 255, 255, 0.5)" stroke-width="2" stroke-linecap="round" stroke-miterlimit="10" d="M4 7h22M4 15h22M4 23h22"/%3E%3C/svg%3E');
  width: 30px;
  height: 30px;
}

.navbar-links {
  display: none; /* Hide by default, will show on larger screens */
}

.navbar-collapse {
  display: none; /* Collapsible menu for smaller screens */
}

.nav-item {
  list-style: none;
}

.nav-link {
  color: white;
  text-decoration: none;
  padding: 8px 16px;
  display: block;
}

@media (min-width: 769px) {
  .navbar-toggler {
    display: none; /* Hide hamburger menu on larger screens */
  }

  .navbar-links {
    display: flex; /* Show direct links on larger screens */
  }
}

@media (max-width: 768px) {
  .navbar-collapse {
    display: block; /* Show collapsible menu on smaller screens */
  }
}
</style>
