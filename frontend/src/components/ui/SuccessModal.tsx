import { useEffect } from "react";

type Props = {
  open: boolean;
  title?: string;
  description?: string;
  primaryText?: string;
  secondaryText?: string;
  onPrimary?: () => void;
  onClose: () => void;
  children?: React.ReactNode;
};

export function SuccessModal({
  open,
  title = "Cliente cadastrado com sucesso!",
  description,
  primaryText = "Cadastrar outro",
  secondaryText = "Fechar",
  onPrimary,
  onClose,
  children,
}: Props) {
  useEffect(() => {
    if (!open) {
        return;
    }

    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === "Escape") {
        onClose();
      }
    };

    document.addEventListener("keydown", onKeyDown);

    return () => document.removeEventListener("keydown", onKeyDown);
  }, [open, onClose]);

  if (!open) {
    return null;
  }

  return (
    <div className="fixed inset-0 z-50">
      <button
        type="button"
        aria-label="Fechar modal"
        onClick={onClose}
        className="absolute inset-0 h-full w-full bg-black/40"
      />

      <div className="relative mx-auto flex min-h-screen max-w-lg items-center justify-center p-4">
        <div className="w-full rounded-2xl bg-white p-6 shadow-xl ring-1 ring-slate-200">
          <div className="flex items-start gap-3">
            <div className="mt-0.5 flex h-10 w-10 items-center justify-center rounded-full bg-emerald-50">
              <svg
                className="h-5 w-5 text-emerald-600"
                viewBox="0 0 20 20"
                fill="currentColor"
                aria-hidden="true"
              >
                <path
                  fillRule="evenodd"
                  d="M16.704 5.29a1 1 0 01.006 1.414l-7.07 7.1a1 1 0 01-1.42.003L3.29 8.88a1 1 0 011.414-1.414l3.21 3.21 6.363-6.387a1 1 0 011.417 0z"
                  clipRule="evenodd"
                />
              </svg>
            </div>

            <div className="flex-1">
              <h3 className="text-lg font-semibold text-slate-900">{title}</h3>
              {description ? (
                <p className="mt-1 text-sm text-slate-600">{description}</p>
              ) : null}
            </div>

            <button
              type="button"
              onClick={onClose}
              className="rounded-md p-2 text-slate-500 hover:bg-slate-100 hover:text-slate-700"
              aria-label="Fechar"
            >
              ✕
            </button>
          </div>

          {children ? <div className="mt-4">{children}</div> : null}

          <div className="mt-6 flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
            <button
              type="button"
              onClick={onClose}
              className="inline-flex items-center justify-center rounded-xl border border-slate-200 bg-white px-4 py-2 text-sm font-medium text-slate-700 hover:bg-slate-50"
            >
              {secondaryText}
            </button>

            {onPrimary ? (
              <button
                type="button"
                onClick={onPrimary}
                className="inline-flex items-center justify-center rounded-xl bg-emerald-600 px-4 py-2 text-sm font-semibold text-white hover:bg-emerald-700"
              >
                {primaryText}
              </button>
            ) : null}
          </div>
        </div>
      </div>
    </div>
  );
}