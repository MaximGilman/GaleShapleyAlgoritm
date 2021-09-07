using System.Linq;
using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace GaleShapleyTests
{
    /// <summary>
    /// Базовый класс для тестов
    /// </summary>
    public abstract class TestsBase
    {

        /// <summary>
        /// Формирует объекты, заполненные тестовыми данными
        /// </summary>
        protected Fixture Fixture { get; }

        protected TestsBase()
        {
            // настройка объекта, формирующего тестовые данные
            Fixture = new Fixture();
            Fixture.Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}