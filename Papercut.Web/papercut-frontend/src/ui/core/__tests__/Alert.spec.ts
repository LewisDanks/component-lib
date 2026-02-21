import { mount } from "@vue/test-utils";
import { describe, expect, it } from "vitest";
import Alert from "../Alert.vue";

describe("Alert", () => {
  it.each(["info", "success", "error"] as const)("renders %s tone", (tone) => {
    const wrapper = mount(Alert, {
      props: {
        tone,
        message: "Status update",
      },
    });

    expect(wrapper.attributes("data-tone")).toBe(tone);
    expect(wrapper.text()).toContain("Status update");
  });
});
