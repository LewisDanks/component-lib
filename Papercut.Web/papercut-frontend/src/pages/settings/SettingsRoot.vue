<script setup lang="ts">
import { reactive, ref } from "vue";
import type { ClientContext } from "../../app/clientContext";
import {
  Alert,
  Button,
  CheckboxInput,
  EmptyState,
  FormField,
  Inline,
  LoadingState,
  PageHeader,
  SelectInput,
  Stack,
  Surface,
  TextInput,
  ToggleInput,
  type AlertTone,
} from "../../ui/core";

const props = defineProps<{
  context: Extract<ClientContext, { pageName: "Settings" }>;
}>();

const formState = reactive({
  displayName: props.context.state.displayName,
  email: props.context.state.email,
  timezone: props.context.state.timezone,
  marketingEmailsEnabled: props.context.state.marketingEmailsEnabled,
  twoFactorEnabled: props.context.state.twoFactorEnabled,
});

const isSaving = ref(false);
const hasSecurityEvents = ref(false);
const alertTone = ref<AlertTone>("info");
const alertMessage = ref("Settings are local-only in this scaffold.");

function saveSettings() {
  isSaving.value = true;

  window.setTimeout(() => {
    isSaving.value = false;
    alertTone.value = "success";
    alertMessage.value = "Settings saved locally.";
  }, 250);
}

function resetSettings() {
  formState.displayName = props.context.state.displayName;
  formState.email = props.context.state.email;
  formState.timezone = props.context.state.timezone;
  formState.marketingEmailsEnabled = props.context.state.marketingEmailsEnabled;
  formState.twoFactorEnabled = props.context.state.twoFactorEnabled;

  alertTone.value = "error";
  alertMessage.value = "Form reset to initial values.";
}

function resetSecuritySession() {
  hasSecurityEvents.value = false;
  formState.twoFactorEnabled = false;

  alertTone.value = "error";
  alertMessage.value = "Security session reset.";
}

function backToDashboard() {
  window.location.assign(props.context.state.dashboardPath);
}
</script>

<template>
  <main>
    <Stack as="section" gap="md">
      <PageHeader
        title="Settings"
        description="Profile, preferences, and security primitives for the new core library."
      />

      <Alert :tone="alertTone" title="Status" :message="alertMessage" />

      <Surface>
        <Stack as="section" gap="md">
          <h2>Profile</h2>

          <FormField id="settings-display-name" label="Display name">
            <TextInput id="settings-display-name" v-model="formState.displayName" />
          </FormField>

          <FormField id="settings-email" label="Email" hint="Demo value for local scaffolding.">
            <TextInput id="settings-email" type="email" v-model="formState.email" />
          </FormField>
        </Stack>
      </Surface>

      <Surface>
        <Stack as="section" gap="md">
          <h2>Preferences</h2>

          <FormField id="settings-timezone" label="Timezone">
            <SelectInput
              id="settings-timezone"
              v-model="formState.timezone"
              :options="props.context.state.timezoneOptions"
            />
          </FormField>

          <CheckboxInput
            id="settings-marketing"
            v-model="formState.marketingEmailsEnabled"
            label="Receive marketing emails"
          />

          <ToggleInput
            id="settings-two-factor"
            v-model="formState.twoFactorEnabled"
            label="Two-factor authentication"
          />
        </Stack>
      </Surface>

      <Surface>
        <Stack as="section" gap="md">
          <h2>Security</h2>

          <EmptyState
            v-if="!hasSecurityEvents"
            title="No active security events"
            message="This scaffold does not load any security timeline yet."
          />

          <Button variant="danger" @click="resetSecuritySession">Reset security session</Button>
        </Stack>
      </Surface>

      <LoadingState v-if="isSaving" label="Saving settings..." />

      <Inline>
        <Button variant="primary" :disabled="isSaving" @click="saveSettings">Save settings</Button>
        <Button variant="secondary" :disabled="isSaving" @click="resetSettings">Reset form</Button>
        <Button variant="secondary" @click="backToDashboard">Back to dashboard</Button>
      </Inline>
    </Stack>
  </main>
</template>
