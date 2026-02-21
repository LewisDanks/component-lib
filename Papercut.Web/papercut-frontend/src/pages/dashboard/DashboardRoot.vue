<script setup lang="ts">
import type { ClientContext } from "../../app/clientContext";

const props = defineProps<{
  context: Extract<ClientContext, { pageName: "Dashboard" }>;
}>();

function signOut() {
  window.location.assign(props.context.state.signOutPath);
}
</script>

<template>
  <main>
    <h1>Dashboard</h1>
    <p>Hello, {{ props.context.state.displayName }}</p>
    <button type="button" @click="signOut">Sign out</button>
  </main>
</template>
