<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from "vue";
import type { ClientContext } from "../../app/clientContext";
import { Button, Inline, PageHeader, Stack, Surface } from "../../ui/core";

const props = defineProps<{
  context: Extract<ClientContext, { pageName: "Dashboard" }>;
}>();

const tick = ref(Date.now());
const serverStartUtcMs = Date.parse(props.context.state.serverUtcIso);
const clientStartMs = Date.now();

const formatter = computed(() => {
  try {
    return new Intl.DateTimeFormat(props.context.state.preferredCulture, {
      dateStyle: "full",
      timeStyle: "medium",
      timeZone: props.context.state.preferredTimeZoneId,
    });
  } catch {
    return new Intl.DateTimeFormat("en-GB", {
      dateStyle: "full",
      timeStyle: "medium",
      timeZone: "UTC",
    });
  }
});

const localizedClock = computed(() => {
  const elapsedMs = tick.value - clientStartMs;
  const currentUtcMs = serverStartUtcMs + elapsedMs;
  return formatter.value.format(currentUtcMs);
});

let timer: number | null = null;

onMounted(() => {
  timer = window.setInterval(() => {
    tick.value = Date.now();
  }, 1000);
});

onBeforeUnmount(() => {
  if (timer !== null) {
    window.clearInterval(timer);
  }
});

function signOut() {
  window.location.assign(props.context.state.signOutPath);
}

function openSettings() {
  window.location.assign(props.context.state.settingsPath);
}
</script>

<template>
  <main>
    <Stack as="section" gap="md">
      <PageHeader title="Dashboard" description="Authenticated landing area." />

      <Surface>
        <Stack as="section" gap="md">
          <p>Hello, {{ props.context.state.displayName }}</p>
          <p data-testid="dashboard-clock">{{ localizedClock }}</p>
          <p>Time zone: {{ props.context.state.preferredTimeZoneId }}</p>
          <p>Locale: {{ props.context.state.preferredCulture }}</p>

          <Inline>
            <Button variant="secondary" @click="openSettings">Open settings</Button>
            <Button variant="danger" @click="signOut">Sign out</Button>
          </Inline>
        </Stack>
      </Surface>
    </Stack>
  </main>
</template>
