<script setup lang="ts">
import { reactive, ref } from "vue";
import { z } from "zod";
import type { ClientContext } from "../../app/clientContext";
import {
  Alert,
  Button,
  CheckboxInput,
  FormField,
  Inline,
  LoadingState,
  PageHeader,
  SelectInput,
  Stack,
  Surface,
  TextInput,
  type AlertTone,
} from "../../ui/core";

const props = defineProps<{
  context: Extract<ClientContext, { pageName: "Settings" }>;
}>();

const SettingsActionResponseSchema = z.object({
  outcome: z.enum(["success", "invalid"]),
  message: z.string(),
});

const formState = reactive({
  displayName: props.context.state.displayName,
  email: props.context.state.email,
  preferredTimeZoneId: props.context.state.preferredTimeZoneId,
  preferredCulture: props.context.state.preferredCulture,
  marketingEmailsEnabled: props.context.state.marketingEmailsEnabled,
});

const twoFactorEnabled = ref(props.context.state.twoFactorEnabled);
const twoFactorCode = ref("");
const isSaving = ref(false);
const alertTone = ref<AlertTone>("info");
const alertMessage = ref("Update preferences and security settings.");

async function postForm(path: string, body: URLSearchParams): Promise<boolean> {
  const response = await fetch(path, {
    method: "POST",
    headers: {
      "Content-Type": "application/x-www-form-urlencoded",
      [props.context.state.antiForgeryHeaderName]: props.context.state.antiForgeryToken,
    },
    body,
  });

  const rawPayload: unknown = await response.json();
  const parsed = SettingsActionResponseSchema.safeParse(rawPayload);

  if (!parsed.success) {
    alertTone.value = "error";
    alertMessage.value = "Unexpected response from settings endpoint.";
    return false;
  }

  alertTone.value = parsed.data.outcome === "success" ? "success" : "error";
  alertMessage.value = parsed.data.message;

  return parsed.data.outcome === "success";
}

async function saveSettings() {
  isSaving.value = true;

  try {
    const body = new URLSearchParams();
    body.set("displayName", formState.displayName);
    body.set("preferredTimeZoneId", formState.preferredTimeZoneId);
    body.set("preferredCulture", formState.preferredCulture);
    body.set("marketingEmailsEnabled", String(formState.marketingEmailsEnabled));

    await postForm(props.context.state.savePreferencesPath, body);
  } catch {
    alertTone.value = "error";
    alertMessage.value = "Failed to save settings.";
  } finally {
    isSaving.value = false;
  }
}

async function enableTwoFactor() {
  isSaving.value = true;

  try {
    const body = new URLSearchParams();
    body.set("code", twoFactorCode.value);

    const success = await postForm(props.context.state.enableTwoFactorPath, body);
    if (success) {
      twoFactorEnabled.value = true;
      twoFactorCode.value = "";
    }
  } catch {
    alertTone.value = "error";
    alertMessage.value = "Failed to enable two-factor authentication.";
  } finally {
    isSaving.value = false;
  }
}

async function disableTwoFactor() {
  isSaving.value = true;

  try {
    const body = new URLSearchParams();

    const success = await postForm(props.context.state.disableTwoFactorPath, body);
    if (success) {
      twoFactorEnabled.value = false;
      twoFactorCode.value = "";
    }
  } catch {
    alertTone.value = "error";
    alertMessage.value = "Failed to disable two-factor authentication.";
  } finally {
    isSaving.value = false;
  }
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
        description="Manage profile, localization preferences, and authenticator-based 2FA."
      />

      <Alert :tone="alertTone" title="Status" :message="alertMessage" />

      <Surface>
        <Stack as="section" gap="md">
          <h2>Profile</h2>

          <FormField id="settings-display-name" label="Display name">
            <TextInput id="settings-display-name" v-model="formState.displayName" :disabled="isSaving" />
          </FormField>

          <FormField id="settings-email" label="Email">
            <TextInput id="settings-email" type="email" v-model="formState.email" :disabled="true" />
          </FormField>
        </Stack>
      </Surface>

      <Surface>
        <Stack as="section" gap="md">
          <h2>Localization</h2>

          <FormField id="settings-timezone" label="Time zone">
            <SelectInput
              id="settings-timezone"
              v-model="formState.preferredTimeZoneId"
              :options="props.context.state.timeZoneOptions"
              :disabled="isSaving"
            />
          </FormField>

          <FormField id="settings-culture" label="Locale">
            <SelectInput
              id="settings-culture"
              v-model="formState.preferredCulture"
              :options="props.context.state.cultureOptions"
              :disabled="isSaving"
            />
          </FormField>

          <CheckboxInput
            id="settings-marketing"
            v-model="formState.marketingEmailsEnabled"
            label="Receive marketing emails"
            :disabled="isSaving"
          />
        </Stack>
      </Surface>

      <Surface>
        <Stack as="section" gap="md">
          <h2>Security</h2>
          <p>Authenticator key: {{ props.context.state.authenticatorSharedKey }}</p>
          <p>Authenticator URI: {{ props.context.state.authenticatorUri }}</p>

          <FormField
            v-if="!twoFactorEnabled"
            id="settings-two-factor-code"
            label="Authenticator code"
            hint="Enter the current code from your authenticator app to enable 2FA."
          >
            <TextInput
              id="settings-two-factor-code"
              v-model="twoFactorCode"
              :disabled="isSaving"
            />
          </FormField>

          <Inline>
            <Button v-if="!twoFactorEnabled" :disabled="isSaving" @click="enableTwoFactor">
              Enable 2FA
            </Button>
            <Button v-else variant="danger" :disabled="isSaving" @click="disableTwoFactor">
              Disable 2FA
            </Button>
          </Inline>
        </Stack>
      </Surface>

      <LoadingState v-if="isSaving" label="Saving settings..." />

      <Inline>
        <Button variant="primary" :disabled="isSaving" @click="saveSettings">Save settings</Button>
        <Button variant="secondary" :disabled="isSaving" @click="backToDashboard">
          Back to dashboard
        </Button>
      </Inline>
    </Stack>
  </main>
</template>
