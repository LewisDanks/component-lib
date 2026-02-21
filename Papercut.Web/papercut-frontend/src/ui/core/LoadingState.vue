<script setup lang="ts">
const props = withDefaults(
  defineProps<{
    label?: string;
  }>(),
  {
    label: "Loading...",
  }
);
</script>

<template>
  <p data-core="LoadingState" role="status">{{ props.label }}</p>
</template>
