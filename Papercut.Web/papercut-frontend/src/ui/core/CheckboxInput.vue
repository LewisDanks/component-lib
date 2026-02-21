<script setup lang="ts">
const props = withDefaults(
  defineProps<{
    id: string;
    label: string;
    modelValue: boolean;
    disabled?: boolean;
  }>(),
  {
    disabled: false,
  }
);

const emit = defineEmits<{
  (e: "update:modelValue", value: boolean): void;
}>();

function onChange(event: Event) {
  const target = event.target as HTMLInputElement;
  emit("update:modelValue", target.checked);
}
</script>

<template>
  <label :for="props.id" data-core="CheckboxInput" class="pc-checkbox-input">
    <input
      :id="props.id"
      type="checkbox"
      :checked="props.modelValue"
      :disabled="props.disabled"
      @change="onChange"
    />
    <span>{{ props.label }}</span>
  </label>
</template>

<style scoped>
.pc-checkbox-input {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}
</style>
