<script setup lang="ts">
const props = defineProps<{
  id: string;
  label: string;
  hint?: string;
  error?: string;
}>();
</script>

<template>
  <div data-core="FormField" class="pc-form-field">
    <label :for="props.id">{{ props.label }}</label>
    <slot />
    <small v-if="props.hint">{{ props.hint }}</small>
    <small v-if="props.error" role="alert">{{ props.error }}</small>
  </div>
</template>

<style scoped>
.pc-form-field {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}
</style>
