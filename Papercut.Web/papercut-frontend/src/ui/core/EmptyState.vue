<script setup lang="ts">
const props = defineProps<{
  title: string;
  message: string;
}>();
</script>

<template>
  <section data-core="EmptyState" class="pc-empty-state">
    <h3>{{ props.title }}</h3>
    <p>{{ props.message }}</p>
  </section>
</template>

<style scoped>
h3,
p {
  margin: 0;
}

.pc-empty-state {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}
</style>
