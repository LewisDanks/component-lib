<script setup lang="ts">
import type { ClientContext } from "../../app/clientContext";
import { Button, Inline, PageHeader, Stack, Surface } from "../../ui/core";

const props = defineProps<{
  context: Extract<ClientContext, { pageName: "Dashboard" }>;
}>();

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

          <Inline>
            <Button variant="secondary" @click="openSettings">Open settings</Button>
            <Button variant="danger" @click="signOut">Sign out</Button>
          </Inline>
        </Stack>
      </Surface>
    </Stack>
  </main>
</template>
