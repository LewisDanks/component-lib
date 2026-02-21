<script setup lang="ts">
import type { AlertTone } from "./types";

const props = withDefaults(
  defineProps<{
    tone?: AlertTone;
    title?: string;
    message: string;
  }>(),
  {
    tone: "info",
    title: "",
  }
);
</script>

<template>
  <aside data-core="Alert" :data-tone="props.tone" class="pc-alert" role="status">
    <strong v-if="props.title">{{ props.title }}</strong>
    <p>{{ props.message }}</p>
  </aside>
</template>

<style scoped>
.pc-alert {
  border: 0.0625rem solid currentColor;
  border-radius: 0.25rem;
  padding: 0.75rem;
}

p {
  margin: 0;
}
</style>
