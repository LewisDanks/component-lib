<script setup lang="ts">
const emit = defineEmits<{
  (e: "login", username: string): void;
}>();

function handleLogin() {
  emit("login", "demo-user");
}
</script>

<template>
  <section class="border p-4 rounded">
    <h2 class="text-lg font-semibold mb-2">Login</h2>
    <button class="bg-blue-600 text-white px-4 py-2 rounded"
            @click="handleLogin">
      Login
    </button>
  </section>
</template>
