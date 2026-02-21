<script setup lang="ts">
const props = defineProps<{
  title: string;
  description?: string;
}>();
</script>

<template>
  <header data-core="PageHeader" class="pc-page-header">
    <h1>{{ props.title }}</h1>
    <p v-if="props.description">{{ props.description }}</p>
    <slot name="actions" />
  </header>
</template>

<style scoped>
.pc-page-header {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

h1,
p {
  margin: 0;
}
</style>
