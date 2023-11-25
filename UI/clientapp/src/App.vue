<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div>
    <nav class="navbar bg-primary" data-bs-theme="dark">
      <div class="container-fluid">
        <RouterLink to="/" class="navbar-brand">Quizes</RouterLink>
        <RouterLink to="/new" class="nav-link">New Component</RouterLink>
      </div>
    </nav>
    <div class="container">
      <RouterView />
    </div>
  </div>
</template>

<style scoped>
/* Your styles here */
</style>
